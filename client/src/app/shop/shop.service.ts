import { environment } from './../../environments/environment';
import { ShopParams } from './../shared/models/shopParams';
import { Category } from '../shared/models/category';
import { Pagination } from '../shared/models/pagination';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {map, retry} from 'rxjs/operators';
import { Product } from '../shared/models/product';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = environment.apiUrl;
  products: Product[] = [];
  categories: Category[] = [];

  constructor(private http: HttpClient) {}

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.categoryId !== 0) {
      params = params.append('categoryId', shopParams.categoryId.toString());
    }



    if(shopParams.search){
      params = params.append('search', shopParams.search);
    }

    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());

    return this.http.get<Pagination>(this.baseUrl + 'products', {
      observe: 'response',
      params,
    }).pipe(
      map(response => {
        this.products = response.body.data;
        return response.body;
      })
    );
  }

  getCategories() {
    if(this.categories.length > 0){
      return of(this.categories);
    }

    return this.http.get<Category[]>(this.baseUrl + 'products/categories').pipe(
      map(response => {
        this.categories = response;
        return response;
      })
    );
  }

  getProduct(id: number){
    const product = this.products.find(p => p.id === id);

    if(product){
      return of(product);
    }
    return this.http.get<Product>(this.baseUrl + 'products/' + id);
  }
}
