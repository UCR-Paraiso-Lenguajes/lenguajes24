import React, { useState, ChangeEvent } from "react";

interface ModalInputProps {
    title: string;
    text: string;
    onInput: (input: string) => void;
}

const ModalInput: React.FC<ModalInputProps> = ({ title, text, onInput }) => {
    const [message, setMessage] = useState<string>('');
    const [input, setInput] = useState<string>('');

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        setInput(e.target.value);
    };

    const handleSetInput = () => {
        onInput(input);
        setMessage('Please wait for our administrator to confirm your method of payment.');
        setInput(''); // Clear input after sending
    };

    return (
        <div className="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabIndex={-1} aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title" id="exampleModalLabel">{title}</h5>
                        <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div className="modal-body">
                        <form>
                            <div className="mb-3">
                                <label htmlFor="recipient-name" className="col-form-label">{text}</label>
                            </div>
                            <div className="form-group">
                                <label htmlFor="recipient-name" className="col-form-label">Please enter the voucher:</label>
                                <input type="text" className="form-control" id="recipient-name" value={input} onChange={handleChange} />
                            </div>
                        </form>
                        {message && <div>{message}</div>}
                    </div>
                    <div className="modal-footer">
                        <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        <button type="button" className="btn btn-primary" onClick={handleSetInput}>Send</button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ModalInput;
