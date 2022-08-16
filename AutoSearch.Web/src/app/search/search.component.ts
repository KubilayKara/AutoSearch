import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ResponseDto, SearchEngine } from '../response.model';
@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  constructor(private http: HttpClient) {

    this.fillSearchEngines();
  }

  ngOnInit(): void {
  }

  results: any[] = [];
  isSearched: boolean = false;
  searchEngines: SearchEngine[] = [];


  fillSearchEngines() {
    //console.log(`Search`);
    //console.log(text);

    let res = this.getSearchEngines().subscribe((data: ResponseDto) => {
      let _response: ResponseDto | undefined;
      _response = { ...data };
      console.log("this._response-1");
      console.log(_response);

      if (_response.isSuccess) {
        this.searchEngines = _response.result as SearchEngine[];
        //alert(_response.result);
        console.log("this.searchEngines");
        console.log(this.searchEngines);


      }
    });
  }

  getSearchEngines() {
    const resp = this.http.get<ResponseDto>(`${environment.AutoSearchApi}/SearchEngines`,);
    console.log("resp");
    console.log(resp);
    return resp;
  }
  search(url: string, text: string, searchEngineIndex: any) {
    //console.log(`searchEngine`);
    console.log(searchEngineIndex);


    let res = this.getSearchResult(url, text, this.searchEngines[searchEngineIndex - 1].id).subscribe((data: ResponseDto) => {
      let _response: ResponseDto | undefined;
      _response = { ...data };
      console.log("this._response-1");
      console.log(_response);

      if (_response.isSuccess) {
        //alert(_response.result);
        this.isSearched = true;
        this.results = _response.result as any[];
      }
    });
  }


  getSearchResult(url: string, text: string, searchEngineId: number) {
    const params = new HttpParams().set('text', text).set('num', 100).set("desiredUrl", url).set('searchEngineId', searchEngineId);
    const resp = this.http.get<ResponseDto>(`${environment.AutoSearchApi}/Search`, { params });
    //console.log("resp");
    //console.log(resp);
    return resp;
  }
}



