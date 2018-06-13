import { Component, OnInit, Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { IssueServices } from '../../services';
import 'rxjs/add/operator/map';

@Component({
    selector: 'counter',
    templateUrl: './counter.component.html'
})
export class CounterComponent {
    constructor(private service: IssueServices, private router: Router, private http: Http) { }
    tickets: any = [];
    isClosed: boolean = false;
    issueID: number = 0;

    ngOnInit() {
        this.service.gettickets()
            .subscribe(
            data => {
                this.tickets = data;
                });
    }

    closeticket(event: any) {
        console.log(event.target.id);
        this.service.closeticket(event.target.id).subscribe(
            data => {
                //ignore data, just reload the list of active tickets
                this.ngOnInit();
            });
    }

    showClosed() {
        this.isClosed = !this.isClosed;
    }
}
