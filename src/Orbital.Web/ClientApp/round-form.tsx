import * as React from 'react';

interface Data {
    variantOfId: string | null,

    category: string | null,
    name: string | null,

    indoor: boolean,
}

interface FormProps {
    data: Data,
    rounds: [{ id: string, name: string }],
}
interface FormState extends Data { }


export default class Form extends React.Component<FormProps, FormState> {
    constructor(props: FormProps) {
        super(props);

        this.state = {
            ...props.data,
        };
    }

    submit(e) {
        e.preventDefault();
    }

    render() {
        return (
            <form onSubmit={e => this.submit(e)}>
                <div className="form-group">
                    <label htmlFor="variantOfId">VariantOf</label>
                    <select name="variantOfId" value={this.state.variantOfId} onChange={e => this.setState({ variantOfId: e.target.value })} className="form-control">
                        <option value="">- Not a variant -</option>
                        {this.props.rounds.map(round => <option value={round.id}>{round.name}</option>)}
                    </select>
                </div>

                <div className="form-group">
                    <label htmlFor="category">Category</label>
                    <select name="category" value={this.state.category} onChange={e => this.setState({ category: e.target.value })} className="form-control">
                        <option value="worldarchery">World Archery</option>
                        <option value="archerygb">ArcheryGB</option>
                    </select>
                </div>

                <div className="form-group">
                    <label htmlFor="name">Name</label>
                    <input name="name" type="text" value={this.state.name} onChange={e => this.setState({ name: e.target.value })} className="form-control" />
                </div>

                <label className="custom-control custom-checkbox">
                    <input type="checkbox" checked={this.state.indoor} onChange={e => this.setState({ indoor: e.target.checked })} className="custom-control-input" />
                    <span className="custom-control-indicator"></span>
                    <span className="custom-control-description">Indoors</span>
                </label>
                {/* TODO targets */}
                <div>
                    <button type="submit" className="btn btn-default">Save</button>
                </div>
            </form>);
    }
}