import re
import sys

paths = re.findall(r'Contador\.[A-Za-z]*/',sys.argv[1])

if paths.__len__() < 1:
    paths = re.findall(r'Contador/[A-Za-z]*',sys.argv[1])

for name in paths:
    r = name.replace('Contador.', '').replace('Contador/', 'Contador.').replace('/', '')
    print(r)

print('')