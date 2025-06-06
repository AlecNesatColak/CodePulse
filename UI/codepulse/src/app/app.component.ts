import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BlogPost } from './features/blog-post/models/blog-post.model';
import { BlogPostService } from './features/blog-post/services/blog-post.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'codepulse';
}
