import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ResponseDto } from '../response.model';
@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }
  _response: ResponseDto | undefined;
  search(text: string) {

    console.log(`Search`);
    console.log(text);

    let res = this.getResult(text).subscribe((data: ResponseDto) => {
      this._response = { ...data }
      console.log("this._response-1");
      console.log(this._response);
      if (this._response.isSuccess)
        alert(this._response.result);
    });
  }
  getResult(text: string) {
    const params = new HttpParams().set('text', text).set('num', 100);

    const resp = this.http.get<ResponseDto>(`${environment.AutoSearchApi}/Search`, { params });
    console.log("resp");
    console.log(resp);
    return resp;
  }
}


