import csv


with open('result.csv', encoding="utf-8") as f:
    reader = csv.reader(f)
    for i in reader:
        if len(i) == 0 or i[1] == 'Цена':
            continue
        print(i)
        i[1] = int(float(i[1][:-2].replace(',', '.').replace(' ', '')) * 100)
        with open('product.csv', 'a', encoding='utf-8', newline='') as g:
            writer = csv.writer(g)
            writer.writerow(i)
