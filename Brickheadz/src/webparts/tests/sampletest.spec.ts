import 'jest';

describe('Simple tests', () => {
  test("1 + 1 = 2", () => {
    expect(1 + 1).toBe(2);
  });

  test("1 + 3 = 4", () => {
    expect(1 + 3).toBe(4);
  });
});
