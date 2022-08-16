export interface ResponseDto {
  isSuccess: boolean
  result: {}
  displayMessage: string
  errorMessaages: [string]
}

export interface SearchEngine {
  id: number,
  title: string,
  url: string,
  regex: string
}
