import React, { Component } from 'react';
import { post } from 'axios';

class UploadForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            id: 'get-id-from-somewhere',
            file: null,
            wordAmount: null, 
        };
    }

    async submit(e) {
        e.preventDefault();

        const url = `https://localhost:5001/api/FileApi/Upload`;
        const formData = new FormData();
        formData.append('body', this.state.file);
        const config = {
            headers: {
                'content-type': 'multipart/form-data',
            },
        };
        return post(url, formData, config).then(response => this.setState({wordAmount: response.data})); //&& console.log( response.data ));
    }

    setFile(e) {
        this.setState({ file: e.target.files[0] });
    }

    render() {
        return (
            <form onSubmit={e => this.submit(e)}>
                <h1>File Upload</h1>
                <input type="file" onChange={e => this.setFile(e)} />
                <button type="submit">Upload</button>
                <div>{this.state.wordAmount}</div>
            </form>
        );
    }
}

export class Counter extends Component {
  static displayName = Counter.name;

  constructor(props) {
    super(props);
    this.state = { currentCount: 0 };
    this.incrementCounter = this.incrementCounter.bind(this);
  }

  incrementCounter() {
    this.setState({
      currentCount: this.state.currentCount + 1
    });
  }

  render() {
    return (
      <div>
        <h1>Counter</h1>

        <p>This is a simple example of a React component.</p>

        <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

        <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
          <UploadForm></UploadForm>
      </div>
    );
  }
}
