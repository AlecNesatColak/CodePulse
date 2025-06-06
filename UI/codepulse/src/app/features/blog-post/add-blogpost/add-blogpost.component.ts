import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { AddBlogpostRequest } from '../models/add-blogpost-request.model';
import { BlogPostService } from '../services/blog-post.service';
import { CategoryService } from './../../category/services/category.service';
import { Category } from '../../category/models/category.model';
import { ImageService } from 'src/app/shared/components/image-selector/image.service';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrls: ['./add-blogpost.component.css'],
})
export class AddBlogpostComponent implements OnInit, OnDestroy {
  model: AddBlogpostRequest;
  private AddBlogPostSubscription?: Subscription;
  categories$?: Observable<Category[]>;
  isImageSelectorVisible: boolean = false;
  private imageSelectorSubscription?: Subscription;

  constructor(
    private router: Router,
    private blogPostService: BlogPostService,
    private categoryService: CategoryService,
    private imageService: ImageService
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

    this.imageSelectorSubscription = this.imageService
      .onSelectImage()
      .subscribe({
        next: (image) => {
          this.model.featuredImageUrl = image.url;
          this.isImageSelectorVisible = false;
        },
      });
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

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnDestroy(): void {
    this.AddBlogPostSubscription?.unsubscribe();
    this.imageSelectorSubscription?.unsubscribe();
  }
}
