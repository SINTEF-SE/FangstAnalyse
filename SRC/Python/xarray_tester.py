# "D:\\Databases\\wind_data\\" + "met_analysis_1_0km_nordic_latest.nc"

import xarray
import pandas as pd

drop_columns = ["air_pressure_at_sea_level", "air_temperature_2m", "altitude", "cloud_area_fraction",
                "forecast_reference_time", "integral_of_surface_downwelling_shortwave_flux_in_air_wrt_time",
                "land_area_fraction", "precipitation_amount", "relative_humidity_2m", "projection_lcc"]
ds = xarray.open_dataset("D:\\Databases\\wind_data\\met_analysis_1_0km_nordic_20190430T12Z.nc")
ds = ds.drop(labels=drop_columns)

print(ds.keys())
