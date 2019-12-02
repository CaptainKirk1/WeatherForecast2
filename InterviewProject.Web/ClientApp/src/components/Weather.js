import React, { Component } from 'react';
import { LocationSelection } from '../components/LocationSelection'

export class Weather extends Component {
    static displayName = Weather.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true, searching: true, locationValue: 'Salt Lake City' };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    searchChange = value => { this.setState({ searching: value }) }
    setLocationValue = value => { this.setState({ locationValue: value }) }

    static renderForecastsTable(forecasts) {
        const iconStyle = { width: '32px' }
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                        <th>Icon</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.date}>
                            <td>{forecast.date}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                            <td><img src={`https://www.metaweather.com/static/img/weather/${forecast.abbreviation}.svg`} alt={forecast.summary} style={iconStyle}></img></td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    static renderForecasts(loading, forecasts) {
        let contents = loading
            ? <p><em>Loading...</em></p>
            : Weather.renderForecastsTable(forecasts);

        return (
            <div>
                <h1 id="tabelLabel" >Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }

    static renderLocationSelection(searchChange, setLocationValue) {
        return (
            <React.Fragment>
                <LocationSelection searchChange={searchChange} setLocationValue={setLocationValue}></ LocationSelection>
            </React.Fragment>
        )
    }

    render() {

        let contents = this.state.searching
            ? Weather.renderLocationSelection(this.searchChange, this.setLocationValue)
            : Weather.renderForecasts(this.state.loading, this.state.forecasts)

        return (
            <React.Fragment>
                {contents}
            </React.Fragment>
        );
    }

    async populateWeatherData() {
        if (!this.state.locationValue) { return }
        const response = await fetch(`weatherforecast?search=${this.state.locationValue}`);
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }


}
