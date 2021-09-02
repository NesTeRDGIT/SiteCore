import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { AppComponent } from "./app.component";
import { TableModule } from "primeng/table";
import { TabViewModule } from 'primeng/tabview';
import { SplitterModule } from 'primeng/splitter';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; 
import { HttpClientModule } from '@angular/common/http';
import { YesNoPipe } from "./YesNoPIPE";

@NgModule({
    imports: [BrowserModule, FormsModule, BrowserAnimationsModule, TableModule, TabViewModule, HttpClientModule, SplitterModule],
    declarations: [AppComponent, YesNoPipe],
    bootstrap: [AppComponent]
})
export class AppModule { }