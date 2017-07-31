import * as React from 'react';

function strEnum<T extends string>(o: Array<T>): {[K in T]: K} {
    return o.reduce((res, key) => {
        res[key] = key;
        return res;
    }, Object.create(null));
}


export const Bowstyle = strEnum([
    'recurve',
    'compound',
    'barebow',
    'longbow'
]);
export type Bowstyle = keyof typeof Bowstyle;

interface Data {
    id: string,
    
    personId: string,
    clubId: string,
    bowstyle: Bowstyle,

    roundId: string,
    competitionId: string,
    shotAt: Date,

    score: number,
    golds: number,   
    hits: number,

    targets: DataTarget[],
}

interface DataTarget {
    roundTargetId: string,

    score: number,
    golds: number,
    hits: number,
}

interface FormProps {
    data: Data,
    submitUrl: string,
}
interface FormState {
    data: Data,
}

export default class RoundForm extends React.Component<FormProps, FormState> {
    render() {
        return (<div>Hello</div>);
    }
}