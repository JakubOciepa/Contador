import re
import sys

paths = re.findall(r'Contador.[A-Za-z]*/',sys.argv[1])

for name in paths:
    r = name.replace('/','').replace('Contador.', '')
    print(r)

print('')