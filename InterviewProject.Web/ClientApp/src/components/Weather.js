import React, { Component } from 'react';

export class Weather extends Component {
  static displayName = Weather.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true, searching: true };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

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

    render() {

        //let contents = this.state.loading
        //    ? <p><em>Loading...</em></p>
        //    : Weather.renderForecastsTable(this.state.forecasts);

        return (
            <React.Fragment>
                {Weather.renderForecasts(this.state.loading, this.state.forecasts)}
            </React.Fragment>
        );
    }




  async populateWeatherData() {
    const response = await fetch('weatherforecast');
    const data = await response.json();
    this.setState({ forecasts: data, loading: false });
  }
}
