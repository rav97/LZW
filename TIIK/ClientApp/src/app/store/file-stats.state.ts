import { State, Action, StateContext, Selector, Select } from '@ngxs/store';
import { GetCharacterPresenceData, LzwEncode, LzwDecode } from './file-stats.action';
import { Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http/';
import { tap } from 'rxjs/operators';
import { IData, ICharactersPresenceData } from '../models/character-presence-data.model';

interface IFileStatsStateModel {
    presenceData: ICharactersPresenceData;
    lzwResult: string;
    compressionRatio: number;
    inputLength: number;
    encodedLength: number;
}

const defaultState: IFileStatsStateModel = {
    presenceData: {
        data: undefined,
        entropy: undefined,
    },
    lzwResult: undefined,
    compressionRatio: undefined,
    inputLength: undefined,
    encodedLength: undefined,
};

@State<IFileStatsStateModel>({
    name: 'fileStats',
    defaults: defaultState,
})
export class FileStatsState {

    @Selector()
    static presenceData(state: IFileStatsStateModel): Array<IData> {
        return state.presenceData.data;
    }

    @Selector()
    static entropy(state: IFileStatsStateModel): number {
        return state.presenceData.entropy;
    }

    @Selector()
    static lzwResult(state: IFileStatsStateModel): string {
        return state.lzwResult;
    }

    @Selector()
    static compressionRatio(state: IFileStatsStateModel): number {
        return state.compressionRatio;
    }

    @Selector()
    static inputLength(state: IFileStatsStateModel): number {
        return state.inputLength;
    }

    @Selector()
    static encodedLength(state: IFileStatsStateModel): number {
        return state.encodedLength;
    }

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    @Action(GetCharacterPresenceData)
    getCharacterPresenceData(ctx: StateContext<IFileStatsStateModel>, action: GetCharacterPresenceData) {
        return this.httpClient.post<ICharactersPresenceData>(this.baseUrl + 'api/entropy', { data: action.text }).pipe(
            tap(x => {
                return ctx.patchState({
                    presenceData: x,
                });
            })
        )
    }

    @Action(LzwEncode)
    lzwEncode(ctx: StateContext<IFileStatsStateModel>, action: LzwEncode) {
        return this.httpClient.post<{ encoded: string[], compressionRatio: number, inputLength: number, encodedLength: number }>
            (this.baseUrl + 'api/encode', { data: action.text }).pipe(
                tap(x => {
                    return ctx.patchState({
                        lzwResult: x.encoded.join(' '),
                        compressionRatio: x.compressionRatio,
                        encodedLength: x.encodedLength,
                        inputLength: x.inputLength,
                    });
                })
            );
    }

    @Action(LzwDecode)
    LzwDecode(ctx: StateContext<IFileStatsStateModel>, action: LzwDecode) {
        return this.httpClient.post<{ decoded: string }>(this.baseUrl + 'api/decode', { data: action.text }).pipe(
            tap(x => {
                return ctx.patchState({
                    lzwResult: x.decoded,
                    compressionRatio: undefined,
                    encodedLength: undefined,
                    inputLength: undefined,
                });
            }),
        );
    }

}
