import matplotlib.pyplot as plt
from pylab import *
import csv
import sys

generation = []
popBestRed = []
popAvgRed = []
bestOverallRed = []

with open(sys.argv[1], 'r') as csvfile:
    plots = csv.reader(csvfile, delimiter=',')
    headers = next(plots)
    print(headers)
    for row in plots:
        try:
            print(row)
            generation.append(int(row[0]))
            popBestRed.append(float(row[1]))
            popAvgRed.append(float(row[3]))
            bestOverallRed.append(float(row[5]))
        except:
            break

plt.plot(generation, popBestRed, label="popBestRed")
plt.plot(generation, popAvgRed, label="popAvgRed")
plt.plot(generation, bestOverallRed, label="bestOverallRed")
plt.legend(loc="lower right")

plt.xlabel(headers[0])
plt.ylabel('Fitness')
plt.title(sys.argv[1])
plt.show()
