import * as React from 'react';
import { render } from 'react-dom';

export default function(el, type, props) {
    render(React.createElement(type, props), el);
}