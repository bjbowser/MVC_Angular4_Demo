import { Component, OnInit, Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { IssueServices } from '../../services';
import 'rxjs/add/operator/map';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})

export class HomeComponent implements OnInit {
    constructor(private service: IssueServices, private router: Router, private http: Http) { }

    types: any = [];
    issue: any = [];
    dateAdded: string = new Date().toLocaleDateString();
    isError: boolean = false;
    issueAdded: boolean = false;

    choseOption(event: any) {
        if (event.target.selectedOptions[0].label == "BUG")
            this.isError = true;
        else
            this.isError = false;
    }

    ngOnInit() {
        this.service.gettypes()
            .subscribe(
            data => {
                this.types = data;
                this.issue.issueTypeID = 4;
                this.issue.dateAdded = new Date().toLocaleDateString();
            });
    }

    addnewissue() {
        this.service.addnewissue(this.issue)
            .subscribe(
            data => {
                this.issue.issueID = data;
                this.issueAdded = true;
            });
    }
}
