import pandas as pd
from sqlalchemy import create_engine

CONNECTION_STRING = 'postgresql://postgres:ffsadkjlda28321a@localhost:5432/fangstanalyse'
TABLE_NAME = 'optimized_catch_data'
CHUNK_SIZE = 50000

CSV_DTYPES = {
    "rundvekt": float,
    "fangstfelt": str,
    "art": str
}

df = pd.read_csv("catch_data_unquoted.csv", dtype=CSV_DTYPES)
df = df[df['rundvekt'].notnull()]
df = df[df['fangstfelt'].notnull()]

# Convert unix to timestamp
df["timestamp"] = pd.to_datetime(df["timestamp"], unit="s")

# Round runvekt to actual rundtvekt. Something in the SQL messes up rounding so:
df["rundvekt"] = df.round({"rundvekt": 2})

# Dump to db
print("All dataframe stuff completed. Starting DB stuff")
try:
    engine = create_engine(CONNECTION_STRING)
    df.to_sql(TABLE_NAME, engine, if_exists="replace", chunksize=CHUNK_SIZE)
except Exception as e:
    print("Failed to add data from file: {file}".format(file="optimized Catch Data"))
    raise e
