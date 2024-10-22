import { NumberFormatPipe } from './number-format.pipe';

describe('NumberFormatPipe', () => {
  let pipe: NumberFormatPipe;

  beforeEach(() => {
    pipe = new NumberFormatPipe();
  });

  it('should create an instance (positive test)', () => {
    expect(pipe).toBeTruthy();
  });

  it('should format number correctly (positive test)', () => {
    const value = 1234567.89;
    const formattedValue = pipe.transform(value);
    expect(formattedValue).toBe('1.234.567,89');
  });

  it('should format integer correctly (positive test)', () => {
    const value = 1234567;
    const formattedValue = pipe.transform(value);
    expect(formattedValue).toBe('1.234.567');
  });

  it('should format zero correctly (positive test)', () => {
    const value = 0;
    const formattedValue = pipe.transform(value);
    expect(formattedValue).toBe('0');
  });

  it('should format negative number correctly (positive test)', () => {
    const value = -1234567.89;
    const formattedValue = pipe.transform(value);
    expect(formattedValue).toBe('-1.234.567,89');
  });

  it('should handle null value (negative test)', () => {
    expect(() => pipe.transform(null as any)).toThrowError();
  });

  it('should handle undefined value (negative test)', () => {
    expect(() => pipe.transform(undefined as any)).toThrowError();
  });

  it('should handle non-number value (negative test)', () => {
    expect(() => pipe.transform('invalid' as any)).toThrowError();
  });
});