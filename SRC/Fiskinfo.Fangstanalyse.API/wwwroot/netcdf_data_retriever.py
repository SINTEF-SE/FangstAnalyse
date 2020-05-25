import xarray
import sys
import numpy as np


class NetCDFDataRetriever(object):

    def __init__(self):
        pass

    # Method to showcase slicing by bbox
    def retrieve_data(self, filename, lat, lon, lat1, lon1):
        ds = xarray.open_dataset(filename)
        subset = ds.sel(longitude=slice(lon, lon1), latitude=slice(lat, lat1))
        # r return sliced data her

    def retrieve_data_by_date(self, filename):
        pass


class DumpNetCDFData(object):

    def __init__(self, filename):
        self.filename = filename

    def retrieve_data(self):
        ds = xarray.open_dataset(self.filename)
        df = ds.to_dataframe()
        #df = df["x_wind_10m", "y_wind_10m"]
        # Lets try downscaling

        g = df.groupby([df.latitude.sub(1) // 2 * 2 + 1.5,
                        df.longitude.sub(1) // 2 * 2 + 1.5])
        new_frame = g["x_wind_10m", "y_wind_10m"].sum().reset_index()
        new_frame.to_csv(sys.stdout, index=False)
        
    def another_retriever(self):
        ds = xarray.open_dataset(self.filename)
        df = ds.to_dataframe()

        # Resample resolution
        step = 1 # Degree
        to_bin = lambda x: np.floor(x / step) * step
        df["latbin"] = df.latitude.map(to_bin)
        df["lonbin"] = df.longitude.map(to_bin)
        groups = df.groupby(("latbin", "lonbin"))  
        groups.to_csv(sys.stdout, index=False)
        
    def yet_another_retriever(self):
        ds = xarray.open_dataset(self.filename)
        df = ds.to_dataframe()

        # Resample resolution
        step = 1 # Degree
        to_bin = lambda x: np.floor(x / step) * step
        df["latbin"] = df.latitude.map(to_bin)
        df["lonbin"] = df.longitude.map(to_bin)
        binned_data = df.groupby(("latbin", "lonbin")).mean()
        binned_data.to_csv(sys.stdout, index=False)

    def create_csv_from_file(self):
        ds = xarray.open_dataset(self.filename)
        df = ds.to_dataframe()

        df.to_csv("all.csv", index=False)


if __name__ == '__main__':
    dumper = DumpNetCDFData(sys.argv[1])
    dumper.retrieve_data()
