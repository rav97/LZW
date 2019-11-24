export class GetCharacterPresenceData {
    static readonly type = '[FileStats] Get character presence data';
    constructor (public text: string) { }
}

export class LzwEncode {
    static readonly type = '[FileStats] Get LZW encode';
    constructor (public text: string) { }
}

export class LzwDecode {
    static readonly type = '[FileStats] Get LZW decode';
    constructor (public text: string) { }
}
