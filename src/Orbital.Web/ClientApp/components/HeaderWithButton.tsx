import * as React from 'react';

export default function ({ title, buttonIcon, buttonText, onClick }) {
    return (
        <div className="d-flex justify-content-end">
            <h2 className="mr-auto my-0 p-2">{title}</h2>
            <div className="p-2">
                <div className="btn-group" role="group">
                    <button className="btn btn-default" onClick={onClick} role="button">
                        <i className={`fa fa-${buttonIcon}`} aria-hidden="true"></i>
                        <div className="sr-only">{buttonText}</div>
                    </button>
                </div>
            </div>
        </div>
    );
}