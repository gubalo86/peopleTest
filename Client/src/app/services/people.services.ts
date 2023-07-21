import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const { baseUrl } = environment;

@Injectable({
  providedIn: 'root',
})
export class PeopleService {
  constructor(private http: HttpClient) {}

  getAll = (): Observable<any> => this.http.get<any>(`${baseUrl}/people`);

  get = (id: number): Observable<any> =>
    this.http.get<any>(`${baseUrl}/people/${id}`);

  post = (model: any): Observable<any> =>
    this.http.post<any>(`${baseUrl}/people`, model);

  put = (model: any): Observable<any> =>
    this.http.put<any>(`${baseUrl}/people/${model.id}`, model);

  delete = (id: number): Observable<any> =>
    this.http.delete<any>(`${baseUrl}/people/${id}`);
}
