import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { FileStatsState } from '../store/file-stats.state';
import { Observable } from 'rxjs';
import { FormGroup, FormControl } from '@angular/forms';
import { LzwEncode, LzwDecode } from '../store/file-stats.action';

@Component({
  selector: 'app-lzw',
  templateUrl: './lzw.component.html',
  styleUrls: ['./lzw.component.css'],
})
export class LzwComponent implements OnInit {

  optionSelected = 'encode';
  options: string[] = ['Encode', 'Decode'];

  @Select(FileStatsState.lzwResult)
  lzwResult$: Observable<string>;

  @Select(FileStatsState.encodedLength)
  encodedLength$: Observable<number>;

  @Select(FileStatsState.inputLength)
  inputLength$: Observable<number>;

  @Select(FileStatsState.compressionRatio)
  compressionRatio$: Observable<number>;

  @Select(FileStatsState.elapsedTime)
  elapsedTime$: Observable<any>;

  inputForm: FormGroup = new FormGroup({
    input: new FormControl(),
  });

  constructor(private store: Store) { }

  ngOnInit() {
    this.store.select(FileStatsState.elapsedTime).subscribe(x => console.log(x));
  }

  submit() {
    const text = this.inputForm.get('input').value;

    if (text == null) {
      return;
    }

    if (this.optionSelected === 'encode') {
      this.store.dispatch(new LzwEncode(text));
    } else if (this.optionSelected === 'decode') {
      this.store.dispatch(new LzwDecode(text));
    }

  }

}
