import os
import httplib2
import numpy as np
from io import BytesIO
from zipfile import ZipFile
from tqdm.auto import tqdm
from bs4 import BeautifulSoup, SoupStrainer



http = httplib2.Http()
def search_for_categories():
    print('Searching for ambientcg categories.')
    categories = []
    status, response = http.request('https://ambientcg.com/categories')
    for link in BeautifulSoup(response, parse_only=SoupStrainer('a')):
        if link.has_attr('href') and link['href'].startswith('./list?category'):
            categories.append(link['href'])
    return categories
        

def search_for_textures(categories):
    print('Searching for ambientcg textures.')
    textures = set()
    for category in tqdm(categories):
        if len(textures) >= 15:
            break
        status, response = http.request(f'https://ambientcg.com/{category}')
        for link in BeautifulSoup(response, parse_only=SoupStrainer('a')):
            if link.has_attr('href') and './view?id' in link['href'] and 'Substance' not in link['href']:
                textures.add(link['href'])
    return textures


def search_for_downloadable_zip_files(textures, resolution='4K'):
    print('Searching for ambientcg downloadable zip files.')
    downloads = set()
    for texture in tqdm(textures):
        if len(downloads) >= 15:
            break
        status, response = http.request(f'https://ambientcg.com/{texture}')
        for link in BeautifulSoup(response, parse_only=SoupStrainer('a')):
            if link.has_attr('href'):
                url = link['href']
                if url.startswith('https://ambientcg.com/get?file') and f'_{resolution}-JPG' in url:
                    downloads.add(url)
    return list(downloads)


def download_and_extract_images(url, skip_existing=True):
    filename = url.split('=')[-1].split('.')[0]
    out_path = f'./Assets/ImageDeformer/Textures/Flooring/{filename}'
    if skip_existing and os.path.exists(out_path):
        print(f'Skipping {out_path}, path already exists.')
        return
    response, content = http.request(url)
    zipfile = ZipFile(BytesIO(content))
    zipfile.extractall(out_path)
                

categories = search_for_categories()
textures = search_for_textures(categories)
downloads = search_for_downloadable_zip_files(textures)
np.random.shuffle(downloads)
print('Downloading files...')
for url in tqdm(downloads):
    download_and_extract_images(url, skip_existing=True)
