import React,  { Component } from 'react';
import FileUpload from './FileUpload';
import {Layout} from "./Layout";
export class App extends Component {
  static displayName = App.name;

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
        <Layout>    
        <h1>Word Counter</h1>

        <p>Push the "Increment" button once you have started the uploading of the file to verify that UI is not blocked</p>

        <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

        <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
          <FileUpload/>    
        </Layout>
    );
  }
}
