import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { AddBlogpostRequest } from '../models/add-blogpost-request.model';
import { BlogPostService } from '../services/blog-post.service';
import { CategoryService } from './../../category/services/category.service';
import { Category } from '../../category/models/category.model';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrls: ['./add-blogpost.component.css'],
})
export class AddBlogpostComponent implements OnInit {
  model: AddBlogpostRequest;
  private AddBlogPostSubscription?: Subscription;
  categories$?: Observable<Category[]>;

  constructor(
    private router: Router,
    private blogPostService: BlogPostService,
    private categoryService: CategoryService
  ) {
    this.model = {
      title: '',
      urlHandle: '',
      shortDescription: '',
      content: '',
      featuredImageUrl: '',
      publishedDate: new Date(),
      author: '',
      isVisible: true,
      categories: [],
    };
  }

  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();
  }

  onFormSubmit(): void {
    console.log(this.model);
    this.AddBlogPostSubscription = this.blogPostService
      .createBlogPost(this.model)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/blogposts');
        },
      });
  }

  ngOnDestroy(): void {
    this.AddBlogPostSubscription?.unsubscribe();
  }
}
