import * as React from 'react';

enum ScoringType { Metric = 1, Imperial, FiveZone, SixZone, Worcester }
enum LengthUnit { Meters = 1, Centimeters, Yards, Feet }

export interface Data {
    scoringType: ScoringType;

    distanceValue: number;
    distanceUnit: LengthUnit;
    faceSizeValue: number;
    faceSizeUnit: LengthUnit;

    arrowCount: number;
}

interface FormProps {
    index: any;
    target: Data;
    onChange: (updatedTarget: Data) => void;
}
interface FormState extends Data { }

export default class RoundTargetForm extends React.Component<FormProps, FormState> {
    constructor(props: FormProps) {
        super(props);

        this.state = {
            ...props.target,
        };
    }

    componentDidUpdate() {
        this.props.onChange(this.state);
    }

    render() {
        return (
            <div className="card my-2">
                <div className="card-block">
                    <h4 className="card-title">Target #{this.props.index}</h4>

                    <div className="form-group">
                        <label htmlFor="scoringType">Scoring</label>
                        <select name="scoringType" value={this.state.scoringType || 0} onChange={e => this.setState({ scoringType: parseInt(e.target.value, 10) })} className="form-control">
                            {Object.keys(ScoringType).filter(key => !isNaN(Number(key)))
                                .map(type => <option key={type} value={type}>{ScoringType[type]}</option>)}
                        </select>
                    </div>

                    <div className="form-group">
                        <label htmlFor="arrowCount">Number of Arrows</label>
                        <input name="arrowCount" type="text" value={this.state.arrowCount || 0} onChange={e => this.setState({ arrowCount: parseInt(e.target.value, 10) })} className="form-control" />
                    </div>

                    <div className="form-group">
                        <label htmlFor="distance">Distance</label>
                        <input name="distance" type="text" value={this.state.distanceValue || ''} onChange={e => this.setState({ distanceValue: parseFloat(e.target.value) })} className="form-control" />
                        <select name="distance_unit" value={this.state.distanceUnit || 0} onChange={e => this.setState({ distanceUnit: parseInt(e.target.value, 10) })} className="form-control">
                            {Object.keys(LengthUnit).filter(key => !isNaN(Number(key)))
                                .map(unit => <option key={unit} value={unit}>{LengthUnit[unit]}</option>)}
                        </select>
                    </div>

                    <div className="form-group">
                        <label htmlFor="faceSize">Face</label>
                        <input name="faceSize" type="text" value={this.state.faceSizeValue || ''} onChange={e => this.setState({ faceSizeValue: parseFloat(e.target.value) })} className="form-control" />
                        <select name="faceSize_unit" value={this.state.faceSizeUnit || 0} onChange={e => this.setState({ faceSizeUnit: parseInt(e.target.value, 10) })} className="form-control">
                            {Object.keys(LengthUnit).filter(key => !isNaN(Number(key)))
                                .map(unit => <option key={unit} value={unit}>{LengthUnit[unit]}</option>)}
                        </select>
                    </div>
                </div>
            </div>
        );
    }
}