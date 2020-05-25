import json
from shapely.geometry import shape, Point

CATCH_AREAS_FILE_NAME = "fangstfelt.json"
COORDINATES_FILE_NAME = "D:\\data\\FTP\\Data\\Wind\\processed_data\\downscaled_csv\\met_analysis_1_0km_nordic_20200417T12Z_downscaled.csv"
CONTAINED_RESULT_OUTPUT_FILE_NAME = "coordinates_contained_in_catch_area_mapping.csv"
CONTAINED_OR_CLOSEST_RESULT_OUTPUT_FILE_NAME = "coordinates_contained_in_catch_area_mapping_or_closest.csv"
CLOSEST_RESULT_OUTPUT_FILE_NAME = "coordinates_closest_to_catch_area_mapping.csv"
CONTAINED_RESULT_FILE_HEADER = "latitude,longitude,coordinate_index,catch_area_id"
CLOSEST_RESULT_FILE_HEADER = "latitude,longitude,coordinate_index,catch_area_id"
DISTANCE_THRESHOLD = 999999


def get_coordinates_from_file(filename):
    points = []
    with open(filename) as file:
        file.readline()  # skip header

        for line in file:
            line_array = line.split(",")
            point = Point(float(line_array[1]), float(line_array[0]))
            point.index = line_array[2]
            points.append(point)

    return points


def get_catch_areas_from_file(filename):
    catch_areas = []

    with open(filename) as file:
        catch_areas_data = json.load(file)
        catch_area_features = catch_areas_data["features"]

        for catch_area in catch_area_features:
            polygon = shape(catch_area['geometry'])
            polygon.catch_area_id = catch_area["properties"]["lok"]
            catch_areas.append(polygon)

    return catch_areas


def map_coordinates_to_catch_areas(catch_areas_file_name, coordinates_file_name):
    catch_areas = get_catch_areas_from_file(catch_areas_file_name)
    points = get_coordinates_from_file(coordinates_file_name)

    with open(CONTAINED_RESULT_OUTPUT_FILE_NAME, "w") as output:
        output.write(CONTAINED_RESULT_FILE_HEADER)
        index = 0

        for coordinate in points:
            for catch_area in catch_areas:
                if catch_area.contains(coordinate):
                    output.write("\n{latitude},{longitude},{index},{catch_area_id}".format(latitude=coordinate.y, longitude=coordinate.x, index=index,
                                                                                           catch_area_id=catch_area.catch_area_id))
                    break

            index += 1


def map_coordinates_to_closest_catch_areas(catch_areas_file_name):
    catch_areas = get_catch_areas_from_file(catch_areas_file_name)
    points = get_coordinates_from_file(CONTAINED_RESULT_OUTPUT_FILE_NAME)

    with open(CLOSEST_RESULT_OUTPUT_FILE_NAME, "w") as output:
        output.write(CLOSEST_RESULT_FILE_HEADER)
        closest_distance = DISTANCE_THRESHOLD
        closest_coordinate = None

        for catch_area in catch_areas:
            for coordinate in points:
                distance = catch_area.centroid.distance(coordinate)

                if distance < closest_distance:
                    closest_distance = distance
                    closest_coordinate = coordinate

            if catch_area.centroid.distance(coordinate) < DISTANCE_THRESHOLD:
                output.write(
                    "\n{latitude},{longitude},{coordinate_index},{catch_area_id}".format(latitude=coordinate.y, longitude=coordinate.x,
                                                                                         coordinate_index=closest_coordinate.index,
                                                                                         catch_area_id=catch_area.catch_area_id))

            closest_distance = DISTANCE_THRESHOLD
            closest_coordinate = None


def map_coordinates_to_containing_or_closest_catch_areas(catch_areas_file_name):
    catch_areas = get_catch_areas_from_file(catch_areas_file_name)
    points = get_coordinates_from_file(CONTAINED_RESULT_OUTPUT_FILE_NAME)

    with open(CONTAINED_OR_CLOSEST_RESULT_OUTPUT_FILE_NAME, "w") as output:
        output.write(CONTAINED_RESULT_FILE_HEADER)
        index = 0

        for catch_area in catch_areas:
            contains = False
            closest_distance = DISTANCE_THRESHOLD
            closest_coordinate = None

            for coordinate in points:
                if not contains and catch_area.contains(coordinate):
                    contains = True
                    output.write("\n{latitude},{longitude},{index},{catch_area_id}".format(latitude=coordinate.y, longitude=coordinate.x, index=index,
                                                                                           catch_area_id=catch_area.catch_area_id))

                distance = catch_area.centroid.distance(coordinate)

                if distance < closest_distance:
                    closest_distance = distance
                    closest_coordinate = coordinate

            if not contains and catch_area.centroid.distance(coordinate) < DISTANCE_THRESHOLD:
                output.write(
                    "\n{latitude},{longitude},{coordinate_index},{catch_area_id}".format(latitude=coordinate.y, longitude=coordinate.x,
                                                                                         coordinate_index=closest_coordinate.index,
                                                                                         catch_area_id=catch_area.catch_area_id))

            index += 1


if __name__ == '__main__':
    # map_coordinates_to_catch_areas(CATCH_AREAS_FILE_NAME, COORDINATES_FILE_NAME)
    # map_coordinates_to_closest_catch_areas(CATCH_AREAS_FILE_NAME)
    map_coordinates_to_containing_or_closest_catch_areas(CATCH_AREAS_FILE_NAME)

