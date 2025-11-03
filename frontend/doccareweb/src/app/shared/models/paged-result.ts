export interface PagedResult<T> {
  page: number;
  pageSize: number;
  totalRecords: number;
  items: T[];
}