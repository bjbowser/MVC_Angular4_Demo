import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'

@Injectable()
export class IssueServices {
    constructor(private http: Http) { }

    gettypes() {
        return this.http.post('/issue/gettypes', {})
            .map((response: Response) => {
                let issueTypes = response.json();
                if (issueTypes) {
                    return issueTypes;
                }
            });
    }

    addnewissue(issue: any) {
        return this.http.post('/issue/addnewissue', {
            IssueID : issue.issueID,
            IssueTypeID: issue.issueTypeID,
            DateAdded: issue.dateAdded,
            IssueText: issue.issueDesc,
            TextRecieved: issue.Recieved,
            TextWanted: issue.Wanted
        })
            .map((response: Response) => {
                let issue = response.json();
                if (issue) {
                    return issue;
                }
            });
    }

    gettickets() {
        return this.http.post('/issue/gettickets', {})
            .map((response: Response) => {
                let issues = response.json();
                if (issues) {
                    return issues;
                }
            });
    }

    closeticket(issueID: number) {
        console.log(issueID);
        return this.http.post('/issue/closeticket', {}, { params: 'IssueID=' + issueID + '' });
    }
}