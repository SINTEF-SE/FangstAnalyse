import psycopg2
import json

CONFIG_FILE_NAME = "config.json"

if __name__ == '__main__':
    with open(CONFIG_FILE_NAME) as config:
        configuration_file = json.load(config)

        username = configuration_file["postgresSettings"]["username"]
        password = configuration_file["postgresSettings"]["password"]
        host = configuration_file["postgresSettings"]["host"]
        port = configuration_file["postgresSettings"]["port"]
        database_name = configuration_file["postgresSettings"]["databaseName"]
        table_name = configuration_file["postgresSettings"]["catchDataStagingTableName"]
        aggregate_catch_data_stored_procedure_name = configuration_file["postgresSettings"]["aggregateMonthlyCatchDataStoredProcedureName"]
        update_temperature_and_air_pressure_stored_procedure_name = configuration_file["postgresSettings"]["UpdateMonthlyCatchWithWeatherDataDataStoredProcedureName"]
        connection_string = str(configuration_file["postgresSettings"]["connectionString"]).format(username=username, password=password, host=host,
                                                                                                   port=port, databaseName=database_name)

        connection = psycopg2.connect(dbname=database_name, user=username, password=password, host=host, port=port)
        cursor = connection.cursor()

        cursor.execute("CALL {stored_procedure}();".format(
            stored_procedure=aggregate_catch_data_stored_procedure_name))
        cursor.execute("CALL {stored_procedure}();".format(
            stored_procedure=update_temperature_and_air_pressure_stored_procedure_name))
        connection.commit()
