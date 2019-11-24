export interface IData {
    letter: string;
    count: number;
    percentage: string;
}

export interface ICharactersPresenceData {
    data: IData[];
    entropy: number;
}
