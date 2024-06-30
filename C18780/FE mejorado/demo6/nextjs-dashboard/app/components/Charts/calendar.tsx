import { useEffect, useState } from 'react';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';

type ValuePiece = Date | null;

type Value = ValuePiece | [ValuePiece, ValuePiece];

export default function MyCalendar({ onSelectDay }: { onSelectDay: any }) {
  const [value, onChange] = useState<Value>(new Date());

  useEffect(() => {
    onSelectDay(value);
  }, [value]);

  return (
    <div>
      <Calendar onChange={onChange} value={value} />
    </div>
  );
}