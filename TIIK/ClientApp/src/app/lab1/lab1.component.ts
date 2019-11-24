import { Component } from '@angular/core';
import { IData } from '../models/character-presence-data.model';
import { Store, Select } from '@ngxs/store';
import { GetCharacterPresenceData } from '../store/file-stats.action';
import { FileStatsState } from '../store/file-stats.state';
import { Observable } from 'rxjs';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-lab1',
  templateUrl: './lab1.component.html',
  styleUrls: ['./lab1.component.css'],
})
export class Lab1Component {
  displayedColumns: string[] = ['letter', 'count', 'percentage'];

  @Select(FileStatsState.presenceData)
  presenceData$: Observable<IData[]>;

  @Select(FileStatsState.entropy)
  entropy$: Observable<number>;

  inputForm: FormGroup = new FormGroup({
    input: new FormControl(),
  });

  constructor(private store: Store) { }

  submit() {
    const text = this.inputForm.get('input').value;

    this.store.dispatch(new GetCharacterPresenceData(text));
  }
}
