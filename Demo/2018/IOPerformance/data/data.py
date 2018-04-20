import pandas as pd
import numpy as np

# レコード数
N = 1000000

chars1 = pd.util.testing.rands_array(5, 100)
chars2 = pd.util.testing.rands_array(5, 10000)

df = pd.DataFrame({'a': np.random.choice(chars1, size=N),
                   'b': np.random.choice(chars2, size=N),
                   'x': np.random.rand(N) * 100,
                   'y': np.random.rand(N) * 100})
df['x'] = df['x'].round(2)
df['y'] = df['y'].round(2)

df.to_csv("test.csv", index=False, float_format='%.2f')
df.to_json('test.json', orient='records', double_precision=2)
