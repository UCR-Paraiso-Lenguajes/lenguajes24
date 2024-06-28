"use client";

import React from 'react';
import { Chart } from 'react-google-charts';

const SalesChart = () => {
  // Define static sales data with product information
  const salesData = [
    ['Datetime', 'Purchase Number', 'Price', 'Amount of Products', { role: 'annotation' }],
    [
      '2024-04-19 09:00:00',
      'P001',
      50,
      5,
      JSON.stringify([
        { name: 'Product A', quantity: 2 },
        { name: 'Product B', quantity: 3 },
      ]),
    ],
    [
      '2024-04-19 10:15:00',
      'P002',
      70,
      3,
      JSON.stringify([{ name: 'Product C', quantity: 3 }]),
    ],
    [
      '2024-04-19 11:30:00',
      'P003',
      60,
      4,
      JSON.stringify([{ name: 'Product D', quantity: 4 }]),
    ],
    [
      '2024-04-19 12:45:00',
      'P004',
      80,
      2,
      JSON.stringify([{ name: 'Product E', quantity: 2 }]),
    ],
    [
      '2024-04-19 14:00:00',
      'P005',
      90,
      6,
      JSON.stringify([{ name: 'Product F', quantity: 4 }, { name: 'Product G', quantity: 2 }]),
    ],
  ];

  // Define static data for weekly sales
  const weeklySalesData = [
    ['Week', 'Sales'],
    ['day 1', 1000],
    ['day 2', 1500],
    ['day 3', 2000],
    ['day 4', 1200],
    ['day 5', 1200],
    ['day 6', 1200],
    ['day 7', 1800],
  ];

  // Render a DataTable chart with the static sales data and child rows for products
  return (
    <div style={{ display: 'flex' }}>
      <div style={{ marginRight: '20px' }}>
        <h2>Sales Chart</h2>
        <Chart
          width={'1000px'}
          height={'300px'}
          chartType="Table"
          loader={<div>Loading Chart</div>}
          data={salesData}
          options={{
            showRowNumber: true,
            cssClassNames: {
              tableRow: 'chart-row',
              headerRow: 'chart-header-row',
              tableCell: 'chart-cell',
            },
            allowHtml: true, // Allows HTML content in cells
          }}
        />
      </div>
      <div>
        <h2>Weekly Sales Pie Chart</h2>
        <Chart
          width={'400px'}
          height={'300px'}
          chartType="PieChart"
          loader={<div>Loading Chart</div>}
          data={weeklySalesData}
          options={{
            title: 'Weekly Sales',
          }}
        />
      </div>
    </div>
  );
};

export default SalesChart;

