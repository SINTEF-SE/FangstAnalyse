"""
    Creates a dataset of averaged monthly wind data using the finer granularity daily wind data value files found in a set repository.
"""

from os import listdir
from os.path import isfile, join
from scipy.constants import convert_temperature
import json
from sqlalchemy import create_engine
import psycopg2
import pandas as pd

REPOSITORY_FILEPATH = "D:\\data\\FTP\\Data\\Wind\\processed_data\\downscaled_csv\\"
OUTPUT_REPOSITORY_FILEPATH = "D:\\data\\FTP\\Data\\Wind\\processed_data\\monthly_averages\\"
TEMPERATURE_AND_CSV_HEADER = "catch_area_id,temperature,air_pressure"
BASE_DATA_FILE_NAME = "met_analysis_1_0km_nordic_"
CATCH_AREA_MAPPING_FILE_NAME = "coordinates_contained_in_catch_area_mapping.csv"
CLOSEST_COORDINATE_MAPPING_FILE_NAME = "coordinates_contained_in_catch_area_mapping_or_closest.csv"
RESULT_FILE_NAME_ENDING = "temperature_and_air_pressure_by_catch_area"
WIND_SPEED_THRESHOLD = 100
TEMPERATURE_THRESHOLD = convert_temperature(50, 'Celsius', 'Kelvin')
AIR_PRESSURE_HIGHER_THRESHOLD = 108500
AIR_PRESSURE_LOWER_THRESHOLD = 87000
AVERAGE_SEA_LEVEL_AIR_PRESSURE = 101325
CONFIG_FILE_NAME = "config.json"
CHUNK_SIZE = 50000


def remove_postfix(text, postfix):
    if text.endswith(postfix):
        return text[:-len(postfix)]
    return text


def get_year_and_month_from_filename(filename):
    """
        Assumes filename are of the form
            met_analysis_1_0km_nordic_{full_year}{full_month}{full_day}T{full_hour}Z_downscaled.csv
    """
    postfix_length = 5
    year_and_month = remove_postfix(filename, "Z_downscaled.csv")[len(BASE_DATA_FILE_NAME):-postfix_length]

    return year_and_month


def get_date_time_string_from_file_name(filename):
    """
        Assumes filename are of the form
            met_analysis_1_0km_nordic_{full_year}{full_month}{full_day}T{full_hour}Z_downscaled.csv
    """
    postfix_length = 5
    year_and_month = remove_postfix(filename, "Z_downscaled.csv")[len(BASE_DATA_FILE_NAME):-postfix_length]

    return "{year}-{month}-01T00:00:00".format(year=year_and_month[:4], month=year_and_month[4:])


def update_value(array, temperature_index, air_pressure_index, values, key):
    temperature = float(array[temperature_index]) if float(array[temperature_index]) < TEMPERATURE_THRESHOLD else 0
    air_pressure = float(array[air_pressure_index])
    air_pressure = air_pressure if AIR_PRESSURE_LOWER_THRESHOLD < air_pressure < AIR_PRESSURE_HIGHER_THRESHOLD else AVERAGE_SEA_LEVEL_AIR_PRESSURE

    if key not in values.keys():
        retval = (temperature, air_pressure, 1)
    else:
        value = values.get(key)
        retval = (value[0] + temperature, value[1] + air_pressure, value[2] + 1)

    return retval


def merge_file_values_and_write_to_database(files, catch_area_mapping, closest_coordinates_mapping, configuration_file):
    values = {}

    for file in files:
        with open("{root_path}{file_name}".format(root_path=REPOSITORY_FILEPATH, file_name=file), "r") as data_file:
            header = data_file.readline().strip().split(",")
            lines = []

            if "datetime" in header:
                header.remove("datetime")

            temperature_index = header.index("air_temperature")
            air_pressure_index = header.index("air_pressure")
            catch_area_mapping_key = 0

            for line in data_file:
                line_array = line.strip().split(",")
                lines.append(line_array)
                key = catch_area_mapping[catch_area_mapping_key] if catch_area_mapping_key in catch_area_mapping.keys() \
                    else None

                # Set data for catch areas that doesn't contain any coordinates when this coordinate is the closest one.
                if catch_area_mapping_key in closest_coordinates_mapping.keys():
                    keys = closest_coordinates_mapping[catch_area_mapping_key]

                    for closest_key in keys:
                        if key == closest_key:
                            continue

                        value = update_value(line_array, temperature_index, air_pressure_index, values, closest_key)
                        values[closest_key] = value

                if key is None:
                    catch_area_mapping_key += 1
                    continue

                temperature = float(line_array[temperature_index]) if float(line_array[temperature_index]) < TEMPERATURE_THRESHOLD else 0
                air_pressure = float(line_array[air_pressure_index])
                air_pressure = air_pressure if AIR_PRESSURE_LOWER_THRESHOLD < air_pressure < AIR_PRESSURE_HIGHER_THRESHOLD else AVERAGE_SEA_LEVEL_AIR_PRESSURE

                if key not in values.keys():
                    values[key] = (temperature, air_pressure, 1)
                else:
                    value = values.get(key)
                    value = (value[0] + temperature, value[1] + air_pressure, value[2] + 1)

                    values[key] = value

                catch_area_mapping_key += 1

    with open("{root_path}{base_file_name}{year_and_month}_{file_name_ending}.csv".format(root_path=OUTPUT_REPOSITORY_FILEPATH,
                                                                                          base_file_name=BASE_DATA_FILE_NAME,
                                                                                          year_and_month=get_year_and_month_from_filename(
                                                                                              files[0]), file_name_ending=RESULT_FILE_NAME_ENDING),
              "w") as output:
        output.write(TEMPERATURE_AND_CSV_HEADER)

        try:
            username = configuration_file["postgresSettings"]["username"]
            password = configuration_file["postgresSettings"]["password"]
            host = configuration_file["postgresSettings"]["host"]
            port = configuration_file["postgresSettings"]["port"]
            database_name = configuration_file["postgresSettings"]["databaseName"]
            table_name = configuration_file["postgresSettings"]["MonthlyAveragedTemperatureAndAirPressureByCatchAreaTableName"]
            truncate_staging_table_stored_procedure_name = configuration_file["postgresSettings"][
                "truncateTemperatureAndAirPressureStagingTableStoredProcedureName"]
            connection_string = str(configuration_file["postgresSettings"]["connectionString"]).format(username=username, password=password,
                                                                                                       host=host, port=port,
                                                                                                       databaseName=database_name)

            # Data in values => db
            values_array = []
            for key, value in values.items():
                #
                # key = catch_area_mapping[catch_area_mapping_key] if catch_area_mapping_key in catch_area_mapping.keys() else None
                #
                # if key is None or key in closest_coordinates_mapping.keys():
                #     pass
                #

                values_array.append((get_date_time_string_from_file_name(files[0]),
                                     key,
                                     convert_temperature(
                                         value[0] /
                                         value[2], 'Kelvin',
                                         'Celsius'),
                                     (value[1] / value[2]) / 100.0))    # Contverting to hPa

            data_frame = pd.DataFrame(values_array, columns=["datetime", "catch_area_id", "temperature", "air_pressure"])
            engine = create_engine(connection_string)
            print("Writing data to database for date {file} ".format(file=get_date_time_string_from_file_name(files[0])))
            data_frame.to_sql(table_name, engine, if_exists="append", chunksize=CHUNK_SIZE, index=False)

            connection = psycopg2.connect(dbname=database_name, user=username, password=password, host=host, port=port)
            cursor = connection.cursor()
            cursor.execute("CALL {stored_procedure}();".format(stored_procedure=truncate_staging_table_stored_procedure_name))
            connection.commit()
        except Exception as e:
            print("Failed to add data from date: {file}".format(file=get_year_and_month_from_filename(files[0])))
            raise e


def get_coordinates_catch_area_mapping():
    catch_area_mappings = {}

    with open(CATCH_AREA_MAPPING_FILE_NAME, "r") as catch_area_mappings_file:
        catch_area_mappings_file.readline()

        for line in catch_area_mappings_file:
            line_array = line.strip().split(",")

            catch_area_mappings[int(line_array[2])] = line_array[3]

    return catch_area_mappings


def get_closest_coordinate_mapping():
    mapping = {}

    with open(CLOSEST_COORDINATE_MAPPING_FILE_NAME, "r") as mappings_file:
        mappings_file.readline()

        for line in mappings_file:
            line_array = line.strip().split(",")

            if int(line_array[2]) not in mapping.keys():
                mapping[int(line_array[2])] = [line_array[3]]
            else:
                mapping[int(line_array[2])].append(line_array[3])

    return mapping


class FileParser(object):
    def __init__(self):
        self.files_to_process = [f for f in listdir(REPOSITORY_FILEPATH) if  # Glob ?
                                 (isfile(join(REPOSITORY_FILEPATH, f)) and f.endswith("downscaled.csv"))]

    def generate_monthly_average_csv_files(self):
        if len(self.files_to_process) == 0:
            return

        current_month_files = []
        current_active_month_and_year = get_year_and_month_from_filename(self.files_to_process[0])

        catch_area_mappings = get_coordinates_catch_area_mapping()
        closest_coordinates = get_closest_coordinate_mapping()
        with open(CONFIG_FILE_NAME) as config:
            configuration_file = json.load(config)

        for i in range(0, len(self.files_to_process)):
            current_month = get_year_and_month_from_filename(self.files_to_process[i])
            print(self.files_to_process[i])

            if current_active_month_and_year != current_month:
                merge_file_values_and_write_to_database(current_month_files, catch_area_mappings, closest_coordinates, configuration_file)
                print("processed files {file_number_start}-{file_number_end}/{number_of_files}".format(
                    file_number_start=i - len(current_month_files), file_number_end=i, number_of_files=len(self.files_to_process),
                    file_name=self.files_to_process[i]))

                current_active_month_and_year = current_month
                current_month_files = [self.files_to_process[i]]
            else:
                print(self.files_to_process[i])
                current_month_files.append(self.files_to_process[i])
                continue

        if len(current_month_files) > 0:
            merge_file_values_and_write_to_database(current_month_files, catch_area_mappings, closest_coordinates, configuration_file)


if __name__ == '__main__':
    reducer = FileParser()
    reducer.generate_monthly_average_csv_files()
