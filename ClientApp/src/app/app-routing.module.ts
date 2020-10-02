import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ProductsComponent } from './products/products.component';


const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot([
    { path: 'home-component', component: HomeComponent },
    { path: 'login-component', component: LoginComponent },
    { path: 'register-component', component: RegisterComponent },
    { path: 'product-component', component: ProductsComponent },
    { path: '', redirectTo: '/home-component', pathMatch: 'full' }// redirect to `first-component`
    //{ path: '**', component: PageNotFoundComponent },  // Wildcard route for a 404 page

  ])],

  exports: [RouterModule]
})
export class AppRoutingModule { }
