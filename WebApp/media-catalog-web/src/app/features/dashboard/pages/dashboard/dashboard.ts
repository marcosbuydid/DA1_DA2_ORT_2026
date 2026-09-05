import { Component } from '@angular/core';
import { Navbar } from '../../../../layout/components/navbar/navbar';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RoleList } from '../../components/role-list/role-list';
import { UserList } from '../../components/user-list/user-list';
import { MovieList } from '../../components/movie-list/movie-list';
import { Footer } from '../../../../layout/components/footer/footer';

@Component({
  selector: 'app-dashboard',
  imports: [Navbar, Footer, CommonModule, RoleList, UserList, MovieList],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {
  items = Array.from({ length: 50 });
  section = 'home';

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.paramMap.subscribe(params => {
      this.section = params.get('section') ?? 'home';
    });
  }
}