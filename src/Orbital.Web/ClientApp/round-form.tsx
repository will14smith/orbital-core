import * as React from 'react';

import HeaderWithButton from './components/HeaderWithButton';

import TargetForm, { Data as TargetData, ScoringType, LengthUnit } from './round-target-form';

interface Data {
    variantOfId: string | null,

    category: string | null,
    name: string | null,

    indoor: boolean,

    targets: TargetData[],
}

interface FormProps {
    data: Data,
    rounds: [{ id: string, name: string }],
    submitUrl: string,
}
interface FormState {
    data: Data,
}

export default class RoundForm extends React.Component<FormProps, FormState> {
    constructor(props: FormProps) {
        super(props);

        this.state = {
            data: {
                ...props.data,
                targets: props.data.targets || [],
            },
        };
    }

    addTarget() {
        this.setState({
            data: {
                ...this.state.data,
                targets: this.state.data.targets.concat({
                    scoringType: ScoringType.metric,

                    distanceValue: 0,
                    distanceUnit: LengthUnit.meters,
                    faceSizeValue: 0,
                    faceSizeUnit: LengthUnit.centimeters,

                    arrowCount: 0,
                }),
            }
        });
    }

    updateTarget(idx, target) {
        const newTargets = [...this.state.data.targets];

        if (newTargets[idx] === target) {
            return;
        }

        newTargets[idx] = target;

        this.setState({
            data: {
                ...this.state.data,
                targets: newTargets,
            }
        });
    }

    submit(e) {
        e.preventDefault();

        fetch(this.props.submitUrl,
            {
                method: 'post',
                credentials: 'same-origin',
                redirect: 'manual',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(this.state.data),
            }).then(response => {
            if (!response.ok) {
                // todo display error
            } else {
                const location = response.headers['Location'];
                window.location.href = location;
            }
        });
    }

    render() {
        return (
            <form onSubmit={e => this.submit(e)}>
                <div className="form-group">
                    <label htmlFor="variantOfId">VariantOf</label>
                    <select name="variantOfId" value={this.state.data.variantOfId || ''} onChange={e => this.setState({ data: { ...this.state.data, variantOfId: e.target.value } })} className="form-control">
                        <option value="">- Not a variant -</option>
                        {this.props.rounds.map(round => <option value={round.id}>{round.name}</option>)}
                    </select>
                </div>

                <div className="form-group">
                    <label htmlFor="category">Category</label>
                    <select name="category" value={this.state.data.category || ''} onChange={e => this.setState({ data: { ...this.state.data, category: e.target.value } })} className="form-control">
                        <option value="worldarchery">World Archery</option>
                        <option value="archerygb">ArcheryGB</option>
                    </select>
                </div>

                <div className="form-group">
                    <label htmlFor="name">Name</label>
                    <input name="name" type="text" value={this.state.data.name || ''} onChange={e => this.setState({ data: { ...this.state.data, name: e.target.value } })} className="form-control" />
                </div>

                <label className="custom-control custom-checkbox">
                    <input type="checkbox" checked={this.state.data.indoor} onChange={e => this.setState({ data: { ...this.state.data, indoor: e.target.checked } })} className="custom-control-input" />
                    <span className="custom-control-indicator"></span>
                    <span className="custom-control-description">Indoors</span>
                </label>

                <hr />

                <HeaderWithButton title="Targets" buttonIcon="plus" buttonText="Create" onClick={() => { this.addTarget(); }} />

                {this.state.data.targets.map((x, i) => <TargetForm key={i} index={i + 1} target={x} onChange={t => this.updateTarget(i, t)} />)}

                <hr />
                
                <div>
                    <button type="submit" className="btn btn-default">Save</button>
                </div>
            </form>);
    }
}