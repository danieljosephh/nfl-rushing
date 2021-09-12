import React, { Component } from 'react';
//import ReactDOM from 'react-dom';
import SampleData from './sampleData.json';
import TableFilter from 'react-table-filter';
import { } from './example.scss';
import { } from './styles.css';

export class RushingStatsTable extends Component {
  constructor(props) {
    super(props);
    this.state = {
      'episodes': SampleData._embedded.episodes,
    };
    this._filterUpdated = this._filterUpdated.bind(this);
  }

  _filterUpdated(newData, filtersObject) {
    this.setState({
      'episodes': newData,
    });
  }

  render() {
    const episodes = this.state.episodes;

    const elementsHtml = episodes.map((item, index) => {
      return (
        <tr key={'row_' + index}>
          <td className="cell">
            {item.name}
          </td>
          <td className="cell">
            {item.season}
          </td>
          <td className="cell">
            {item.number}
          </td>
        </tr>
      );
    });

    return (
      <div>
        {/* <div className="nav-bar">
          <div className="container">
            NFL Rushing Stats
          </div>
        </div> */}
        <div className="examples">
          <div className="basic">
            <h3 className="header">NFL Rushing Stats</h3>
            <table className="basic-table">
              <thead>
                <TableFilter
                  rows={episodes}
                  onFilterUpdate={this._filterUpdated}>
                  <th key="name" filterkey="name" className="cell" casesensitive={'true'} showsearch={'true'}>
                    Name
                  </th>
                  <th key="season" filterkey="season" className="cell">
                    Season
                  </th>
                  <th key="number" filterkey="number" className="cell" alignleft={'true'}>
                    Number
                  </th>
                </TableFilter>
              </thead>
              <tbody>
                {elementsHtml}
              </tbody>
            </table>
          </div>

        </div>
      </div>
    );
  }
}