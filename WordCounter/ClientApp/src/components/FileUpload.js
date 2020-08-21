import React, {useState, useEffect} from 'react';
import axios from "axios";
import Loader from 'react-loader-spinner'
import constants from '../constants/constants'

function FileUpload(){
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState(false);
    const [error, setError] = useState();
    const [file, setFile] = useState();
    
    // handleSubmit can be used in a separate service later on
    const handleSubmit = (e) => {
        e.preventDefault();
        const { url } = constants;
        const formData = new FormData();
        formData.append('body', file);
        const config = {
            headers: {
                'content-type': 'multipart/form-data',
            },
        };
        setLoading(true);
        axios.post(url, formData, config).then(r => {
            setResult(r.data);
            setLoading(false);
        }).catch((error) => {
            console.error(error);
        });
    };

    return (
        <form onSubmit={(e) => handleSubmit(e)}>
            <h1>File Upload</h1>
            <input type="file" onChange={e => setFile(e.target.files[0])} />
            <button type="submit">Upload</button>
            <div>{result}</div>
            <div>{error}</div>
            { loading ? 
                <Loader
                type="Puff"
                color="#00BFFF"
                height={100}
                width={100}
            />:null } 
        </form>
    );
}
export default FileUpload;