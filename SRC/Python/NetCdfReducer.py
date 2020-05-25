import json
from os import listdir, path
from os.path import isfile, join

import xarray

CONFIG_FILE_NAME = "config.json"
PROCESSED_FILES_REGISTRY_FILE_PATH = "processed_files.json"
CONFIG_PROPERTY_DOWNSCALED = "reduced_files"


def remove_postfix(text, postfix):
    if text.endswith(postfix):
        return text[:-len(postfix)]
    return text


class NetCDFReducer(object):
    def __init__(self, configuration_file):
        self.configuration_file_path = configuration_file
        with open(self.configuration_file_path) as default_config:
            self.configuration_file = json.load(default_config)
        self.processed_files = json.load(open(PROCESSED_FILES_REGISTRY_FILE_PATH, 'r')) if isfile(PROCESSED_FILES_REGISTRY_FILE_PATH) else {
            CONFIG_PROPERTY_DOWNSCALED: []}
        self.repository_directory = self.configuration_file["windData"]["windDataRepositoryDirectory"]
        all_files = [join(self.repository_directory, f) for f in listdir(self.repository_directory) if  # Glob ?
                     (isfile(join(self.repository_directory, f)) and f.endswith(".nc"))]
        self.files_to_process = []

        for file in all_files:
            if "reduced" in file:
                continue
            if file not in self.processed_files[CONFIG_PROPERTY_DOWNSCALED]:
                self.files_to_process.append(file)

    def perform_reduction(self):
        for file in self.files_to_process:
            self.reduce_file(path.join(self.repository_directory, file))

    def update_reduced_files_registry_on_disk(self):
        with open(PROCESSED_FILES_REGISTRY_FILE_PATH, "w") as registry_file:
            json.dump(self.processed_files, registry_file)

    def reduce_file(self, filename):
        print("reducing file: " + filename)
        ds = xarray.open_dataset(filename)
        drop_columns = ["altitude", "cloud_area_fraction",
                        "forecast_reference_time", "integral_of_surface_downwelling_shortwave_flux_in_air_wrt_time",
                        "land_area_fraction", "precipitation_amount", "relative_humidity_2m", "projection_lcc", "x",
                        "y"]
        print("Attempting to drop columns")
        ds = ds.drop(labels=drop_columns)
        output_name = remove_postfix(filename, ".nc")
        output_name += "_reduced.nc"
        ds.to_netcdf(output_name)
        # self.configuration_file["processed_files"].append(filename)
        self.processed_files[CONFIG_PROPERTY_DOWNSCALED].append(filename)
        self.update_reduced_files_registry_on_disk()

    def convert_reduced_file_to_csv(self, filename):
        ds = xarray.open_dataset(filename)
        df = ds.to_dataframe()
        df.to_csv(filename.replace("_reduced.nc", ".csv"))


if __name__ == '__main__':
    reducer = NetCDFReducer(configuration_file=CONFIG_FILE_NAME)
    reducer.perform_reduction()
