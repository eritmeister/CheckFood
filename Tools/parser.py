import csv
import requests
from bs4 import BeautifulSoup
import re

url = 'https://www.perekrestok.ru/cat'


def get_categories_url():
    response = requests.get(url)
    soup = BeautifulSoup(response.text, 'html.parser')
    categories_table = soup.find('div', class_='catalog__list')
    categories_html = categories_table.find_all('a', class_='sc-fFubgz fsUTLG')
    categories_hrefs = []
    for category in categories_html:
        categories_hrefs.append(category.get('href'))
    formated_categories_hrefs = categories_hrefs[4:-7]
    formated_categories_hrefs.pop(-3)
    formated_categories_hrefs.pop(-3)
    formated_categories_hrefs.remove('/cat/mc/229/tovary-dla-mam-i-detej')
    print(formated_categories_hrefs)
    return formated_categories_hrefs


def get_sub_categories_hrefs(categories_hrefs):
    sub_categories_hrefs = []
    for category_href in categories_hrefs:
        c = 0
        i = 0
        print(category_href)
        response = requests.get('https://www.perekrestok.ru' + category_href)
        soup = BeautifulSoup(response.text, 'html.parser')
        sub_categories_html = soup.find_all('div', class_='products-slider__header')
        for sub_category_html in sub_categories_html:
            link = sub_category_html.find('a').get('href')
            sub_categories_hrefs.append(link)
            print(link)
            print("_")
            i += 1
        if i == 0:
            with open(f'{c}.html', mode='w', encoding="utf-8") as f:
                f.write(response.text)
            c += 1
    sub_categories_hrefs = list(set(sub_categories_hrefs))
    if '/viewed-products' in sub_categories_hrefs:
        sub_categories_hrefs.remove('/viewed-products')
    return sub_categories_hrefs


def get_products_info(sub_categories_hrefs):
    products_info = []
    count = 1
    product_count = 0
    final_count = len(sub_categories_hrefs)
    for sub_category in sub_categories_hrefs:
        product_count = 0
        response = requests.get('https://www.perekrestok.ru' + sub_category)
        soup = BeautifulSoup(response.text, 'html.parser')
        main_table = soup.find('div', class_='catalog-content')
        product_table = main_table.find('div', class_='sc-laRPJI fcDJqq')
        products_html = product_table.find_all('div', class_='sc-gHftXq gEpYQD')
        img_name = ""
        for product_html in products_html:
            try:
                product_name = product_html.find('div', class_='product-card__title').text
                product_price_html = product_html.find('div', class_='price-new').text
                img_url = product_html.find('img', class_='product-card__image').get('src')
                img_name = re.sub(r'[/\\"]&;:{}', '_', product_name)
                if '\t' in img_name:
                    img_name = " ".join(img_name.split("\t"))
                with open(f"images/{img_name}.jpg", "wb") as img:
                    image = requests.get(img_url)
                    img.write(image.content)
                products_info.append({
                    "name": product_name.strip(),
                    "price": product_price_html.strip(),
                    "img_url": img_url
                })
                product_count += 1
            except AttributeError:
                pass
            except OSError:
                pass
        print(f"{count}/{final_count} - {sub_category}")
        count += 1
    print(products_info)
    return products_info


def write_product_info(product_list):
    with open('result.csv', mode='w', encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(["Название товара", "Цена", "Фотография товара"])
        for product in product_list:
            try:
                writer.writerow(product.values())
            except:
                pass


def main():
    categories_url = get_categories_url()
    subcategories_url = get_sub_categories_hrefs(categories_url)
    print(subcategories_url)
    print("\n")
    print(len(subcategories_url))
    subcategories_url = list(set(subcategories_url))
    product_info = get_products_info(subcategories_url)
    write_product_info(product_info)


if __name__ == '__main__':
    main()
