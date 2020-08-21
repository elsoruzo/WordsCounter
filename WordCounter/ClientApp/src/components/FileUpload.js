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
    const handleSubmit = async (e) => {
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
          let response = await axios.post(url, formData, config);
           setResult(response.data);
           setLoading(false);

    };

    return (
        <form onSubmit={(e) => handleSubmit(e)}>
            <h1>File Upload</h1>
            <input type="file" onChange={e => setFile(e.target.files[0])} />
            <button className="btn btn-primary" type="submit">Upload</button>
            <div>{result}</div>
            <div>{error}</div>
            { loading ? 
                <Loader
                type="Circles"
                color="#00BFFF"
                height={80}
                width={80}
            />:null } 
        </form>
    );
}
export default FileUpload;