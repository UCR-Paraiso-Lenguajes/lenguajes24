'use client'
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { Chart } from "react-google-charts";

export const options = {
    title: "My Daily Activities",
};

export default function Pie({ data }: { data: (string | number)[][] }) {
    return (
        <><Chart
            chartType="PieChart"
            data={data}
            options={options}
            width={"400px"}
            height={"400px"}
        /></>
    );
}