'use client'
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { Chart } from "react-google-charts";

export const options = {
    allowHtml: true,
    showRowNumber: true,
    legend: { position: "bottom" },
    pageSize: 50,
};

export const formatters = [
    {
        type: "NumberFormat" as const,
        column: 3,
        options: {
            prefix: "₡",
            negativeColor: "red",
            negativeParens: true,
        },
    },
    {
        type: "NumberFormat" as const,
        column: 5,
        options: {
            prefix: "₡",
            negativeColor: "red",
            negativeParens: true,
        },
    },
];

export default function Charts({ data }: { data: (string | number)[][] }) {
    return (
        <>
            <Chart
                chartType="Table"
                width="100%"
                height="100%"
                data={data}
                options={options}
                formatters={formatters}
            />
        </>
    );
}