import { Injectable } from '@angular/core';
import { AddBlogpostRequest } from '../models/add-blogpost-request.model';
import { Observable } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { UpdateBlogPostRequest } from '../models/update-blogpost-request.model';

@Injectable({
  providedIn: 'root',
})
export class BlogPostService {
  constructor(private http: HttpClient) {}

  createBlogPost(data: AddBlogpostRequest): Observable<BlogPost> {
    return this.http.post<BlogPost>(`${environment.apiBaseUrl}/blogpost`, data);
  }

  getAllBlogPosts(): Observable<BlogPost[]> {
    return this.http.get<BlogPost[]>(`${environment.apiBaseUrl}/blogpost`);
  }

  getBlogPostById(id: string): Observable<BlogPost> {
    return this.http.get<BlogPost>(`${environment.apiBaseUrl}/blogpost/${id}`);
  }

  getBlogPostByUrlHandle(urlHandle: string): Observable<BlogPost> {
    return this.http.get<BlogPost>(
      `${environment.apiBaseUrl}/blogpost/blogpost-details/${urlHandle}`
    );
  }

  updateBlogPost(
    id: string,
    data: UpdateBlogPostRequest
  ): Observable<BlogPost> {
    return this.http.put<BlogPost>(
      `${environment.apiBaseUrl}/blogpost/${id}`,
      data
    );
  }

  deleteBlogPost(id: string): Observable<void> {
    return this.http.delete<void>(`${environment.apiBaseUrl}/blogpost/${id}`);
  }
}
